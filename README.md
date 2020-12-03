# baUHInia!
Markdown goes brrrrrrr
## Server module
Virtual machine has to be up and running in the background if you want anything to work as intended.
Download link: https://drive.google.com/file/d/16Xemlih9VBp3iTSB30bMb8BpKMVnbyOz/view?usp=sharing
### Usage
1. Download.
2. Import.
3. Run.
### System update
1. Don't.
### Server update
1. Switch to TTY2 (Ctrl+Alt+F2).
2. Open terminal and type in `cd node-io;git pull`
4. Server will restart automatically after successful pull (nodemon magic).
5. But just in case, switch to TTY1 (Ctrl+Alt+F1) and make sure the output is "green".
6. If it failed (only if we introduced some dependency, which may never happen) press Ctrl+C.
### Putting your filthy thingers on the database
1. Switch to TTY2.
2. In the MongoDB Compass window click on "Fill in connection fields individually".
3. "Connect".
4. Do stuff.
### Miscellaneous
Tested under VirtualBox 6.1.16. Older versions might break the "VBox doesn't capture my mouse when I don't want it to" functionality also known as VBox Guest Additions. To enable VBox Guest Additions you have to switch to TTY2 at least once.
