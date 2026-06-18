using System.Windows;
using System.Windows.Media.Animation;
class AnimLib
{
    public static IEasingFunction Quad { get; set; } = new QuadraticEase
    {
        EasingMode = EasingMode.EaseInOut
    };

    public static IEasingFunction Sine { get; set; } = new SineEase
    {
        EasingMode = EasingMode.EaseInOut
    };

    public static IEasingFunction Back { get; set; } = new BackEase
    {
        EasingMode = EasingMode.EaseInOut
    };

    public static IEasingFunction Bounce { get; set; } = new BounceEase
    {
        EasingMode = EasingMode.EaseInOut
    };

    public static IEasingFunction Circle { get; set; } = new CircleEase
    {
        EasingMode = EasingMode.EaseInOut
    };

    public static IEasingFunction Cubic { get; set; } = new CubicEase
    {
        EasingMode = EasingMode.EaseInOut
    };

    public static IEasingFunction Elastic { get; set; } = new ElasticEase
    {
        EasingMode = EasingMode.EaseInOut
    };

    public static IEasingFunction Exponential { get; set; } = new ExponentialEase
    {
        EasingMode = EasingMode.EaseInOut
    };

    public static IEasingFunction Power { get; set; } = new PowerEase
    {
        EasingMode = EasingMode.EaseInOut
    };

    public static IEasingFunction Quartic { get; set; } = new QuarticEase
    {
        EasingMode = EasingMode.EaseInOut
    };

    public static IEasingFunction Quintic { get; set; } = new QuinticEase
    {
        EasingMode = EasingMode.EaseInOut
    };

    public static void ObjectMove(DependencyObject Object, Thickness Get, Thickness Set, int MilliSeconds = 750)
    {
        Storyboard Board = new Storyboard();
        ThicknessAnimation Animation = new ThicknessAnimation
        {
            EasingFunction = Quad,
            From = Get,
            To = Set,
            AutoReverse = false,
            Duration = new Duration(TimeSpan.FromMilliseconds(MilliSeconds))
        };

        Storyboard.SetTarget(Animation, Object);
        Storyboard.SetTargetProperty(Animation, new PropertyPath(FrameworkElement.MarginProperty));

        Board.Children.Add(Animation);
        Board.Begin();
    }

    public static void Fade(DependencyObject Object, double From = 0, double To = 1, int MilliSeconds = 500)
    {
        Storyboard Board = new Storyboard();
        DoubleAnimation Animation = new DoubleAnimation()
        {
            From = From,
            To = To,
            AutoReverse = false,
            Duration = new Duration(TimeSpan.FromMilliseconds(MilliSeconds))
        };

        Storyboard.SetTarget(Animation, Object);
        Storyboard.SetTargetProperty(Animation, new PropertyPath("Opacity", 1));

        Board.Children.Add(Animation);
        Board.Begin();
    }

    public static void DoubleAnimation(DependencyObject Object, string Property, double From = 0, double To = 1, int MilliSeconds = 500)
    {
        Storyboard Board = new Storyboard();
        DoubleAnimation Animation = new DoubleAnimation()
        {
            From = From,
            To = To,
            AutoReverse = false,
            Duration = new Duration(TimeSpan.FromMilliseconds(MilliSeconds))
        };

        Storyboard.SetTarget(Animation, Object);
        Storyboard.SetTargetProperty(Animation, new PropertyPath(Property, 1));

        Board.Children.Add(Animation);
        Board.Begin();
    }
}