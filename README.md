# Tournament
Creates and tracks a tournament.

This uses a .net core project to keep the models and helps set up entity framework to work with the database. The .net core porject is then used by the asp.net core project to set up a web app and will eventually be used for mobile apps that want to connect through the api in the asp.net core project.

This is by no means production ready. I am also looking into creating apps for different types of tournaments ie: sports, board games, video games, etc. If anyone is interested in creating an app for a specific tournament create an issue.

I am currently working on a basic tournament app for uwp and porting over a specialized tournament app I created.

Additionally I have not yet tested the api part of the asp.net core project (Tournaments controller) so that might not work as expected.

#Known Issues
There is currently no way to add a description, players (unless substituting players for teams), or individual games inside each match.

If a tournament has lots of teams it will crash due to an excess of inputs being submitted form a form.
