using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace screensaver {
    public partial class MainWindow {

        /// <summary>
        /// Global parameters:
        /// Threshold: euclidian distance between dots in which they start to connect
        /// Refreshspeed: time in ms to refresh screen
        /// _color: color of particles 
        /// backgroundColor: color of canvas
        /// </summary>
        private const int Threshold = 200;
        private const int NumberOfParticles = 100;
        private const int RefreshSpeed = 33;
        private Color _color = Color.FromArgb(0, 82, 148, 226);
        private readonly Brush _backgroundColor = new SolidColorBrush(Color.FromRgb(48, 54, 66));

        public MainWindow() {
            InitializeComponent();
            Frame.Background = _backgroundColor;
            var random = new Random();
            var particles = Enumerable.Range(0, NumberOfParticles).Select(element => new Particle(random, _color)).ToList();
            Draw(particles);

            #if !DEBUG
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            WindowState = WindowState.Maximized;
            KeyDown += (sender, e) => Environment.Exit(0);
            #endif
        }
        /// <summary>
        /// Draw the particles and the lines between them
        /// </summary>
        /// <typeparam name="T">Can be any particles class derived from particle interface</typeparam>
        /// <param name="particles">List of particles</param>
        private async void Draw<T> (List<T> particles) where T : IParticle {
            while (true) {
                particles.ForEach(particle => { 
                    particle.Update();
                    Canvas.SetTop(particle.Form, particle.Position.Y);
                    Canvas.SetLeft(particle.Form, particle.Position.X);
                    Frame.Children.Add(particle.Form);

                    particles.ForEach(neighbor => {
                        var distance = EuclidianDistance(particle.Position, neighbor.Position);
                        if (!(distance < Threshold)) return;
                        var adjustParticle = particle.Size / 2.0;
                        var adjustNeighbor = neighbor.Size / 2.0;
                        _color.A = (byte)(255 - distance / Threshold * 255);
                        Frame.Children.Add(new Line {
                            X1 = particle.Position.X + adjustParticle,
                            X2 = neighbor.Position.X + adjustNeighbor,
                            Y1 = particle.Position.Y + adjustParticle,
                            Y2 = neighbor.Position.Y + adjustNeighbor,
                            Stroke = new SolidColorBrush(_color),
                            StrokeThickness = 1
                        });
                    });
                });
                await Task.Delay(RefreshSpeed);
                Frame.Children.Clear();
            }
        }

        private static double EuclidianDistance(Point a, Point b) => Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    }

    public interface IParticle {
        void Update();
        Shape Form { get; }
        Point Position { get; }
        int Size { get; }
    }

    public class Particle : IParticle {
        private Point _position;
        private Point _velocity;
        private readonly Ellipse _form;
        private readonly int _size;

        public Particle(Random random, Color color) {
            _position.X = random.Next(0, (int)SystemParameters.PrimaryScreenWidth);
            _position.Y = random.Next(0, (int)SystemParameters.PrimaryScreenHeight);
            _velocity.X = random.Next(-100, 101) / 200.0;
            _velocity.Y = random.Next(-100, 101) / 200.0;

            color.A = (byte) random.Next(128, 255);
            _size = random.Next(3, 6);
            _form = new Ellipse {
                Fill = new SolidColorBrush(color),
                Width = _size,
                Height = _size
            };
        }

        public void Update() {
            _position.X += _velocity.X;
            _position.Y += _velocity.Y;

            if (_position.X > SystemParameters.PrimaryScreenWidth || _position.X < 0) _velocity.X *= -1;
            if (_position.Y > SystemParameters.PrimaryScreenHeight || _position.Y < 0) _velocity.Y *= -1;
        }

        Shape IParticle.Form => _form;
        Point IParticle.Position => _position;
        int IParticle.Size => _size;
    }
}
