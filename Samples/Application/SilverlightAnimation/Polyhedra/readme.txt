The basic way the Polyhedron animation works in this app is as follows:

(1) A graph describing how the faces are connected is loaded from a resource
(2) This graph is used to generate a two dimensional Net - basically the equivalent
    of a cardboard cut-out.
(3) This Net is then used to generate Faces in 3 dimensions that can be folded to
    form the final Polyhedron
(4) This Polyhedron is then "projected" onto the xaml canvas using a perspective
    transformation. The z-order of the polygons corresponding to the individual
    faces is set so that they draw from back to front. They are made semi-transparent
    so you can see the whole solid.

The xaml for the wheel of polyhedrons in Page1.xaml is auto-generated using code similar
to this app with Projector.Project replaced with a Project.ProjectToXaml.

I had quite a bit of fun using Storyboards in this app to fade-in and magnify the 
currently selected Polyhedron and to rotate the arrow on the cycle button.

Triggers are only available for the Load event in this Alpha. All other events have
to be wired manually to storyboards. Watch out for the MouseEnter event which is generated
multiple times (so a flag is needed to prevent the Storyboard reseting continuously).

You are welcome to use most of the code in this app as you see fit. 
The DirectX Math emulation library is probably the most reusable part and is available
under the GNU Lesser General Public License. Most of the rest of the app is copyright
free. However I'm retaining copyright on Graphs.cs, Net.cs and Polyhedron.cs because
I use more elaborate version of these in another app. (Please don't use them without
permission.)

Have fun with Silverlight. I certainly did.

Declan Brennan
declan@brennan.name