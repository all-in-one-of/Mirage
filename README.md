Mirage (VRDG+H2 version)
------------------------

This is a branch for another version of Mirage that was used at VRDG+H2 on 30th
April 2016. For details of the original project, please see [the master branch]
[Master].

This time the visuals are controlled with Ableton Live, which was also used for
playing back the pre-recorded audio track.

![Screenshot][Screenshot]

A Max for Live device called [Livegrabber][Livegrabber] was used for
establishing communication between Live and Unity. It sends OSC messages with
notes and automation curves in the Live project. It also analyzes audio level
of audio tracks and sends data of them.

I also used [fgc.reenableautomation][FGC] to work around [an issue of Live]
[Issue]. That's a really nasty bug, but you can shut it up with this device.

The Live project is in [the Ableton directory][LiveProject] without M4L
devices and audio clips. To open the project with Live, create a empty
directory named “Devices” and put Livegrabber and fgc.reenableautomation in
it. Or you can simply use “search missing files” feature in Live.

I can’t distribute the audio clips... that’s secret sauce of the project :yum:
Put any track you like instead of them.

[Master]: https://github.com/keijiro/Mirage
[Livegrabber]: http://showsync.info/tools/livegrabber/
[FGC]: http://www.maxforlive.com/library/device/3351/fgc-reenableautomation
[Issue]: https://cycling74.com/forums/topic/re-enable-automation-turning-on-randomly
[Screenshot]: https://66.media.tumblr.com/6587fe54f29aaecedc1995726e196991/tumblr_o6h9dgv8Ta1qio469o1_400.png
[LiveProject]: https://github.com/keijiro/Mirage/tree/external-control/Ableton
