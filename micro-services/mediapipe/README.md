# StudioControlGestureRecognition.MediapipeMicroService
A microservice accessable AI for the StudioControlGestureRecognition System, written in Python using FastAPI and Uvicorn.

See [StudioControlGestureRecognition](https://github.com/Robbe-Vr/StudioControlGestureRecognition)

NOTE: if the commands ```pip``` or ```pipenv``` have been installed but do not run directly from the commmand shell -> include ```python -m ``` infront of each command.
A command will then look as follows: e.g ```python -m pipenv install <package>```.

```pip``` will install a package globally, available to everywhere on the system where python runs.
```pipenv``` creates a virtual environment to store packages for specifically and only the current project.

## Install Instructions
Open a command shell, change the directory to the projects directory.
Having pip and pipenv installed, run ```pipenv install``` to install all required dependencies (packages).

## Running
Open a command shell, change the directory to the projects directory.
Having pip installed, run ```pip install uvicorn[standard]```.
Either make sure the uvicorn command is globally available in your command shell ```uvicorn```, or use ```python -m uvicorn``` to use uvicorn to run and host the FastAPI web api.

Now run ```uvicorn app.server:app --port <PORT> --reload``` to run the application on the specified port and enable hot-reloads.
Having hot-reloads enabled, uvicorn will now look for file changes in the projects directory and automatically restart the web api server to include the changes when file changes have been detected.
Include ```--host 0.0.0.0``` to make the application available on your internal network instead of localhost.

## Deployment
Open a command shell, change the directory to the projects directory.
Make sure pip is installed.
serve can host the frontend application similar to the ```npm start``` command, except it will run in production mode instead of debug/development mode. (debug mode will show very detailed error logs to the end user, which can include source code or other information that you may want to have kept in secrecy)
If you want to host the application from the shell directly on your system, make sure serve is installed globally and is available as a direct command in your command shell.
If so, first run the command ```npm build```.
This will compile the TypeScript code into a JavaScript react application. REMINDER: the "npm build" command creates a build version of the application that cannot be managed by npm anymore on deployment, this means that the environment variables as defined in the ".env" file will NOT be used. These values should either be hardcoded into the application or served from a different location available to the compiled version of the application.

Once the build has finished, you should see a "build" folder has been created in the projects root directory.
Now you can run ```serve -s build```, you can also specify a port by including the flag with value ```-p <PORT>```, otherwise the default port will be 3000.

Another option is to use docker. A docker file to build and host the application is already included in this project: [Dockerfile](Dockerfile).
Having installed docker on the local machine, open a command shell, change the directory to the projects directory, and run: ```docker build -t studiocontrolgesturerecognitionapp .```.
After this command has completed, run: ```docker run -d -p <PORT>:3000 --name StudioControlGestureRecognitionApp --restart always studiocontrolgesturerecognitionapp``` to start hosting the application on Docker.
the values "studiocontrolgesturerecognitionapp" and "StudioControlGestureRecognitionApp" are simply names used to identify the build and application name in the list of Docker hosted applications on the machine, these can be changed into any other value accordingly.
