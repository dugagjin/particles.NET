# particles.NET

When I saw [this](https://marcbruederlin.github.io/particles.js/) or [this](http://vincentgarreau.com/particles.js/) it looked so cool that I wanted it as screensaver. 

<img src="https://media.giphy.com/media/xT0xenwzp5JdB6Obss/giphy.gif">

If you want the same screensaver as the one shown on the GIF then you can download the *defautScreenSaver.scr* file added to this repository. Once donwloaded you can right click on it and to choose install.

## Create screensaver from the code

- Open the solution
- Set Visual Studio to release
- Build
- Go to the release folder where the .exe is
- Replace the .exe with .scr 
- Right click on the .scr and choose install

## Options

You can change de global parameters to customize it.

```
private const int Threshold = 200;
private const int NumberOfParticles = 100;
private const int RefreshSpeed = 33;
private Color _color = Color.FromArgb(0, 82, 148, 226);
private readonly Brush _backgroundColor = new SolidColorBrush(Color.FromRgb(48, 54, 66));
```

- Threshold: distance in pixels in which the lines start to be visible
- NumberOfParticles: self-explainatory
- RefreshSpeed: The time between frames in ms
- _color: color of the lines
- _backgroundColor: color of the canvas


## Advanced 

If you don't like dots but want to have other shapes such as a star then you can create a new class which extends to this interface:

```
public interface IParticle {
    void Update();
    Shape Form { get; }
    Point Position { get; }
    int Size { get; }
}
```

Once done you only have to change this:

```
var particles = Enumerable.Range(0, NumberOfParticles).Select(element => new Particle(random, _color)).ToList();
```

To:

```
var particles = Enumerable.Range(0, NumberOfParticles).Select(element => new YOURCUSTOMCLASS(...)).ToList();
```