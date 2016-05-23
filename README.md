# FastFileAccessExtension

Download this extension from the [VS Gallery](https://visualstudiogallery.msdn.microsoft.com/e214ce6a-f47c-494a-b0a3-8e1adbd0dd5e)

---------------------------------------

Fast file access within big projects or solutions.

Search in big solutions with several project and files for one specific file can be annoying.
To make this easier I wrote this extension. With this extension you can search with several 
types of searches within all files which are part of the loaded soltion and projects.
<br/><br/>
The extension can be used with the keyboard. With the keys Ctrl+R,D you can open the search
and start typing. With the Up and Down keys you can change from the search box to the listview.
<br/>

| Command                        | Keybinding     |
|:------------------------------ |:-------------- |
| Open search                    | Ctrl+R, Ctrl+D |
| Cahnge from search to listview | Up or Down key |
| Open selected file             | Return Key     |

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
- [x] Search type settings
- [x] Display settings

### Show a window which contains all the files from Solution Explorer
Displays all the files within the loaded solutions and projects in the 
Fast File Access Window. If only the filenames or e.g. also the projects
are displayed can be configured in the settings.<br/>
<img src="Images/FastFileAccessWindow.png" width="400" /><br/>

### Add search to the file window
In the list of all files can be search using the search box. Several different
types of search algorithms can be used for searching within the files.<br/>
<img src="Images/FastFileAccessWindowSearch.png" width="400" /><br/>

### Open file from choose one in the window
The selected file can be opened in the editor by pressing the return button or
double click the item.  <br/>
<img src="Images/FastFileAccessWindowOpen.png" width="400" /><br/>

### Search type setting
In the settings of Visual Studio the type of the search can be configured.<br/>
<img src="Images/SettingsSearchType.png" width="400" /><br/>

| Setting                     | Description                                      |
|:--------------------------- |:------------------------------------------------ |
| 1. Search type              | Search algorithm which should be used            |
| 2. Ignore case              | Case sensitive or un-sensitive search            |
| 3. Starts with              | Text should start with the given search string   |
| 4. Max Levenshtein distance | Max. distance which is still recognised as match |

### Display setting
Defines what should be displayed in the file list and therefor also for which
you are able to search for.<br/>
<img src="Images/SettingsDisplayType.png" width="400" /><br/>

| Setting             | Description                              |
|:------------------- |:---------------------------------------- |
| 1. Add project name | Add the project name to each file        |
| 2. Add file paths   | Add the full directory path to each file |

## Contribute
Check out the [contribution guidelines](CONTRIBUTING.md)
if you want to contribute to this project.

## License
[Apache 2.0](LICENSE)