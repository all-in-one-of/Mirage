Mirage (VRDG+H2 version)
------------------------

This is a branched version of *Mirage*, which was used at “VRDG+H2” on 30th
April 2016. For details of the original project, please refer to [the master
branch][Master].

This time the visuals are controlled from [Ableton Live][Ableton], which was
also used for playing back the pre-recorded audio track.

![Screenshot][Screenshot]

A [Max for Live][M4L] device called [Livegrabber][Livegrabber] was used to make
communication between Live and Unity. It sends OSC messages from MIDI notes and
automation curves in MIDI tracks. It also analyzes audio level of audio tracks
and sends data from them.

Besides that a Max 4 Live device called [fgc.reenableautomation][FGC] was used
to resolve [an issue in M4L][Issue]. It’s a nasty bug but can be shut up with
this device anyway.

The Live project is located in the [Ableton directory][LiveProject] but without
these M4L devices. To add the devices, create an empty directory named “Devices”
in the project directory, then put files from Livegrabber and
fgc.reenableautomation into the “Devices” directory.

I used Livegrabber v3.3.3 and fgc.reenableautomation v1.1 with Ableton Live
v9.6.1. You may use later versions, but try to ensure to use same versions in
case any issues arise.

Audio clips (soundtrack) are also missing from the Live project... that’s
the secret sauce of the project :yum: Put any tracks you like instead of them.

[Master]: https://github.com/keijiro/Mirage
[Ableton]: https://www.ableton.com
[M4L]: https://www.ableton.com/en/live/max-for-live/
[Livegrabber]: http://showsync.info/tools/livegrabber/
[FGC]: http://www.maxforlive.com/library/device/3351/fgc-reenableautomation
[Issue]: https://cycling74.com/forums/topic/re-enable-automation-turning-on-randomly
[Screenshot]: https://66.media.tumblr.com/6587fe54f29aaecedc1995726e196991/tumblr_o6h9dgv8Ta1qio469o1_400.png
[LiveProject]: https://github.com/keijiro/Mirage/tree/external-control/Ableton
