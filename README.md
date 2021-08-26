# Datalogger-Alistair
The GUI application for interfacing with a Steer Energy Datalogger using a local PC

## Table of Contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Installation](#installation)
* [Development](#development)
* [Issues](#issues)

## General Info
This project is a Windows Form application which allows users to interface with a Steer Energy Datalogger. This program allows the user to create/import log configs on their local machine, upload them to the logger, download and process logs from the logger as well as some other features. It has customisable preset configuration options as well as the ability to export data files in various formats inclduing .csv, .zip and .xlsx. If you want to download the application, please go [here](#installation)

## Technologies
This project was written mainly in C# (with the exception of the python processing scripts) and was created with:
* Visual Studio Community 2019 Version 16.10.4
* PyCharm 2021.2 (Communnity Edition)
* Advanced Installer Version 18.5
and is designed to run on any Windows PC running Windows Vista or higher

## Installation
You can easily install the project by going to the [releases page](https://github.com/steerenergy/Datalogger-Alistair-UI/releases) and downloading the .msi installer for the latest release. Run the installer and follow the instructions and the program will be installed on your computer. To connect to a logger, make sure there is a Steer Energy Datalogger running on the same network as your computer, then run the application and connect using the connection form.

## Development
If you wish to develop this project, you will need to be added to the steerenergy organisation and you will need to speak to Alistair-L-R or NickRyanSteer to get the required documents. **Note: It is unlikely this project will need development unless Steer Energy directly ask for it and have a need for further development.**

## Issues
If you encounter an issue whilst using the application, please report the issue [here](https://github.com/steerenergy/Datalogger-Alistair-UI/issues) and we will try to fix it as soon as possible. In your report, please include these details if applicable:
* The version of the software
* Error messages - screenshots ideal
* Screenshots/Photos to show issues
* Any data/config files used in the testing
* Steps to reproduce the issue
* 'piError.log' for a Logger Issue

Any questions, message Alistair-L-R
Happy Logging!
