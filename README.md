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

MIDI Controller Mappings
------------------------

![Controls][Map1]

![CC][Map2]

Note:

- *Spikes* and *Shafts* are fired by the kick trigger. These signal paths are
  enabled only when the knob values are higher than 50%.
- The intensity levels of the light sources are controlled by the envelopes
  (fired by the kick and snare triggers) and the bias sliders.
- The statue deformers are controlled by the envelopes and the bias buttons.

OSC Triggers
------------

*Mirage* receives OSC messages on UDP port 7000, 8000 and 9000. It listens
only on the following three addresses:

- /kick (float value) - fires the kick trigger on rising edges (0 to 1)
- /snare (float value) - fires the snare trigger on rising edges (0 to 1)
- /clock (int value) - fires the tick trigger

Define Symbols
--------------

There are two build-time options to switch the behavior of the software. These
options are set with Scripting Define Symbols in the player settings.

- MIRAGE_TEST - shows the test menu on the primary display.
- MIRAGE_TRIPLE - uses the second and third display instead of the first and
  second display. In this mode the first display is only used for the test menu.

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
[Map1]: http://40.media.tumblr.com/13668faf9f766cbf7815074645d9fd68/tumblr_o4r19v1qlk1qio469o1_r1_640.png
[Map2]: https://40.media.tumblr.com/d74bc6d3bc6d7d49fa4e89218617d948/tumblr_o4r19v1qlk1qio469o2_640.png
[Stanford]: http://graphics.stanford.edu/data/3Dscanrep/
[AnavriN]: https://www.cgtrader.com/free-3d-models/character-people/fantasy/lucy-a-christian-angel-statue
