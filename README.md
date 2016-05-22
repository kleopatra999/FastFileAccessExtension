# FastFileAccessExtension

Download this extension from the [VS Gallery](https://visualstudiogallery.msdn.microsoft.com/[GuidFromGallery])

---------------------------------------

Fast access files within big projects or solution.

Search in big solutions with several project and files for one specific file can be annoying.
To make this easier I wrote this extension. With this extension you can search with several 
types of searches within all files which are part of the loaded soltion and projects.
<br/><br/>
The extension can be used with the keyboard. With the keys Ctrl+R,D you can open the search
and start typing. With the Up and Down keys you can change from the search box to the listview.
<br/><br/>
Keybinding:<br/>
- Open search: Ctrl+R,D<br/>
- Change focus from search to listview: Up or Down key<br/>
- Open selected file: Return Key<br/>
<br/>
Current search or string matching types:<br/>
- Contain string<br/>
- Regex matching<br/>
- Levenshtein distance<br/>
- Word base levenshtein distance

See the [changelog](CHANGELOG.md) for changes and roadmap.

## Features

- [x] Show a window which contains all the files from Solution Explorer
- [x] Add search to the file window
- [x] Open file from choose one in the window
- [x] Search type setting
- [x] Display setting

### Show a window which contains all the files from Solution Explorer
<img src="Images/FastFileAccessWindow.png" width="400" /><br/>

### Add search to the file window
<img src="Images/FastFileAccessWindowSearch.png" width="400" /><br/>

### Open file from choose one in the window
<img src="Images/FastFileAccessWindowOpen.png" width="400" /><br/>

### Search type setting
<img src="Images/SettingsSearchType.png" width="400" /><br/>

### Display setting
<img src="Images/SettingsDisplayType.png" width="400" /><br/>

## Contribute
Check out the [contribution guidelines](CONTRIBUTING.md)
if you want to contribute to this project.

## License
[Apache 2.0](LICENSE)