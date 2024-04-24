Change necessary:

To use the program, it is necessary to use the CMD command <<sqllocaldb create "Local">>, which creates a local db using SSMS named Local, with the path "(localDB)\Local".

If the Entity Framework is not installed, run the following command in power shell <<dotnet tool install --global dotnet-ef --version 8.*>>. This will  insatll EF with the latest version 8.

Changing directories is necessary with <<cd ./ExpressYourself>>, as the sln file in not in the project file.

Finally, run the command <<dotnet ef database update>> to create the database tables.
