Mirage
======

*Mirage* is a holographic live visuals software, which was originally made for
[DUB-Russell][DR]’s live performance. It uses [the Pepper’s ghost technique]
[Wikipedia] to make an illusion that the objects actually exist on the stage.

![Gif][Gif1] ![Gif][Gif2]

It was initially performed at [DMM VR Theater][DMM] “[VRDG+H][VRDG]” on 26th
March 2016.

System Requirements
-------------------

- Unity 5.3.4 or later
- Windows system with multiple display-out
- [Novation Launch Control XL][Novation] (replaceable with other MIDI
  controllers)

*Mirage* uses the multi-display feature, which was newly introduced in Unity
5.3. Although this feature was implemented in a platform-agnostic fashion, it
doesn’t work property on OS X because of [an issue][Issue]. In conclusion, it
needs a Windows system at the moment.

The main scene was set up based on the actual stage design of the theater. This
is the most essential part to make the illusion real. It should be re-adjusted
when performing with another venue.

Acknowledgement
---------------

The “Lucy” statue model was originally scanned by [Stanford University Computer
Graphics Laboratory][Stanford], and retopologized by [anavriN][AnavriN]. The
original files are freely available from each page.

[DR]: http://dubrussell.com
[Wikipedia]: https://en.wikipedia.org/wiki/Pepper%27s_ghost
[Gif1]: http://49.media.tumblr.com/13c6797008e2dc0e08c14ea7650a6d8b/tumblr_o44krsnvkn1qio469o1_320.gif
[Gif2]: http://45.media.tumblr.com/a627871cc513d124bd700000229bfdf3/tumblr_o4jutbvM351qio469o1_320.gif
[DMM]: http://www.dmm.com/theater/en
[VRDG]: http://brdg.tokyo
[Novation]: https://global.novationmusic.com/launch/launch-control-xl
[Issue]: https://issuetracker.unity3d.com/issues/multi-display-feature-doesnt-work-on-os-x
[Stanford]: http://graphics.stanford.edu/data/3Dscanrep/
[AnavriN]: https://www.cgtrader.com/free-3d-models/character-people/fantasy/lucy-a-christian-angel-statue
