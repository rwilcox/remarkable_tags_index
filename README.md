# About

This - currently - is a command line application that lists all your tags and the notebooks and pages where they live.

## The Remarkable's Tagging problem

I'm a heavy Remarkable tags user, with 60+ tags. Problem is that in the page tag picker in landscape mode the Remarkable doesn't display more than 30 some odd, and in portrait orientation will display about 50-60.

But, not enough.

Thankfully the Remarkable's Tags Menu option will display as many tags as I have, by sacrificing page display space, but will not allow you to edit a page's tags from there.

## But, you say, does the desktop or mobile app help?

Currently these apps don't support tags.

# Problems this app solves

I want a list of all my tags, and notebook names and page numbers, on the desktop, to work around the above two constraints.

# Setup

## Slithin

You can use [Slithin](https://github.com/furesoft/Slithin/) to transfer your files to your computer

**First**: Synchronize your Remarkable with your computer and note where Slithin stores the folder (probably in your Documents folder, which may or may not be on your OneDrive).

**Second**: (and I hope to get binary releases soon): build this app. If you have the DotNET SDK installed you should be able to `dotnet build`. Since .NET Core is everywhere this should work on MacOS X and Linux too.

The binary is now found in `bin/Debug/net7.0/`

**Third**: Run the application: `tags_explorer`. It takes a single argument: the path to the Notebooks folder Slithin saved for you

Example: `.\tags_explorer.exe "C:\Users\rwilcox\OneDrive\Documents\Slithin\Devices\Mine\Notebooks"`

## SCP

You can also use `scp`. An example command is:

**First**: Syncronize your Remarkable with your computer via this shell command (or a variant)

    scp -r root@$REMARKABLE_IP_ADDRESS:~/.local/share/remarkable/xochitl/ ~/Documents/Remarkable_Backup/

**Second**: (same as above)

**Third**: Run the application, except the argument is the location you saved your files to, ie: `tags_explorer ~/Documents/Remarkable_Backup/`

# Results

The results of the program should be something like this!

    remarkable
    functional
    javascript
    emacs
    ================================================
    Count of tags: 4
    ================================================

    functional
      * Inboz page 13
      * Inboz page 15
      * Dev Synthesis page 1
      * Future blog posts page 8
    javascript
      * Inboz page 13
    emacs
      * Mini Projects page 37
      * Future blog posts page 9
      * Future blog posts page 11
    remarkable
      * Finances page 21
      * Mini Projects page 7


# Disclaimer!

While I'm relatively experienced in other languages, this is my first from scratch .NET project! I like that the Remarkable desktop tools community uses C# a lot, so I wrote this in C# to hope to reuse some of what others have written, or incorporate parts of this into their projects.

The compiler / runtime will spit a lot of warnings at you! The compiler can't check the null safety of some of my code, but it seems to work for me!

# Future plans

Ideally here's the checklist of things I want to do. But I might never!:

  - [ ] hook up CI (CircleCI?) to build and push Github releases for major OSes
  - [ ] Hook a UI up to it?? Even just an outline view?
  - [ ] ... if I have the page uuids and have gone through all this work already, can I let people title their pages, even if it's just in my software? The UUID that's currently page 13 of the Inboz, can we associate a more descriptive name with it?
