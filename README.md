# JList
Create  List objects that work inside the file system and say bye to high memory usage.
All the data is stored in a folder.
## Quick Start
### Install..
#### Install-Package jfolderlist2 -Version 1.0.0
#### dotnet add package jfolderlist2 --version 1.0.0
### Initialize with GUID or a Text you want
#### Use GUID
```c#
//Initialize with GUID
JList<string> ls = new JList<string>(); //It will automatically create a list in the data folder with a folder random folder name, but please use a custom name based list for more presistence
ls.Add("Hie");
ls.Add("How are you?");

var datapath = ls.path;//You should store this, it is needed for reinitialization
//Re-Init Example
JList<string> ls = new JList<string>("retrive");// you should always use retrive to retrive data from a path
ls.path = datapath; // Here we successfully retrived the whole data
```
#### Use custom dir
```c#
//Initialize with Custom Text
JList<string> ls = new JList<string>("testlist");// this name is presistent as long as the data folder is in the same path as the app.
ls.Add("Hie");
ls.Add("How are you?");
JList<string> ls2 = new JList<string>("testlist");// this won't create another data file but will use the data file that's already created which means it retrived the data again.

```
#### Init from oridinary List<>
```c#
//Initialize with an old list
List<string> list = List<string>("testlist");// this is a simple list.
list.Add("Hie");
list.Add("How are you?");
JList<string> jlist = new JList<string>(list);// This data is automatically saved into the data folder with a folder name of "list.GetHashCode();"
//You can save the path of the data as the first example

```
### Liscence
This project is under the GPLv3
