This project includes two ways of doing "fullscreen" applications.

You choose between the two different ways by commenting in the one you like in the App.xaml.cs file.
this.RootVisual = new Fullscreen1();
//this.RootVisual = new Fullscreen2();

Fullscreen1 - Shows how to make an application fullscreen/browser by setting "styles" on 
the controls to make them resize properly.

Fullscreen2 - Shows how to scale up your application by code.

It's two different ways of doing it as I see it and is used in different scenarios. The way it's done
in Fullscreen1 will typically be used for "application", meaning an application with a "static" layout.

The way it's done in Fullscreen2 is more for "games", as it scales the hole application up, which can 
constain lots of different dynamic controls at the time.

Well, this is just the way I see it at the moment please let me know if you think otherwise, I 
could be wrong. :)

Visit laumania.net
