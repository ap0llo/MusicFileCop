MusicFileCop
============

[![Build status](https://ci.appveyor.com/api/projects/status/9cf9uyom74neukg6?svg=true)](https://ci.appveyor.com/project/ap0llo/musicfilecop)

MusicFileCop is a tool that aids the management of music collections. 
It searches a directory for music files and applies a set of rules on them.

All violations of rules are written to a report (currently that's just a text file)

For example, a rule makes sure that a release year has been specified for every album.


Usage
--------

Apply rules to all music files in *PATH* (recursively) and writes the violations to the file *OUT*

	MusicFileCop.exe check --path PATH --out OUT


Write the full default configuration to the specified path

	MusicFileCop.exe export-default-config --out OUT
	

List all rules with a description

	MusicFileCop.exe list-rules


Configuration
--------------
The configuration allows to influence the behavior of certain rules and enable/disable rules altogether.
MusicFileCop uses a hierarchical configuration model, different configuration files are "overlayed".

Configuration files are JSON files that can be added at both directory or file level. Directory configuration 
files change the configuration for all subdirectories and all files within that directory.

If a more specific configuration is found, all settings specified in it override inherited values, 
all other values are taken from the higher-level configuration.

The "root" configuration is the default configuration specified by each rule in its implementation
 
Directory configuration files need to be named *MusicFileCop.json*, 
configuration for a single file needs to be named *FILENAME.MusicFileCop.json* and placed next to the file itself. 
 
###Example
 	directory
	   |-directory1
	   		|-file1.mp3
	   		|-file1.mp3.MusicFileCop.json	
	   |-directory2
            |-directory3
				 |-file2.mp3
            |-MusicFileCop.json	   
	   |-directory4
	   		|-file3.mp3
 
 
In the example above, there are two configuration files placed within the directory structure. 
There is no configuration file in either the root directory or *directory4*, so the default configuration 
will be used for all files within these directories (and all files except *file1.mp3* in *directory1*)

For *file1.mp3* all settings specified in *file1.mp3.MusicFileCop.json* will override values from the default configuration.

Analogous, values from *MusicFileCop.json* will overlay the default configuration and the combined
configuration will be applied to all files within *directory2* or any of its sub-directories


To aid the creation of configuration files, the default configuration can be written to a file using the *export-default-config* command

### Example: Disabling a rule
To disable a rule for a certain directory, add the following to a configuration file (the names of the rules 
can be listed using the *list-rules* command

	{
		"ConsistencyChecker": {
			"AlbumMustHaveArtist": {
			"Enabled": "True"
			}   
		}
	}


Acknowledgements
----------------
MusicFileCop uses a number of third-party libraries
- [Ninject](http://www.ninject.org/)
- [Ninject.Extensions.Conventions](https://github.com/ninject/ninject.extensions.conventions)
- [NLog](http://nlog-project.org/)
- [taglib#](https://github.com/mono/taglib-sharp)
- [Json.NET](http://www.newtonsoft.com/json)
- [ASP.Net 5 Configuration Framework](https://github.com/aspnet/Configuration)
- [CommandLineParser](https://github.com/gsscoder/commandline)


For testing
- [xunit](http://xunit.github.io)
- [Moq](https://github.com/Moq/moq4)