## Data Faker

This application was used to populate test databases (MySQL). These databases were than used with another script in order to populate MongoDB databases with different mappings.

## Running application

Update ``App.config`` file before running, as it contains the connection string for each database context.

After launching the application the user is prompted to enter which data model should be populated.

Even though there a several options all of them are variants of 2 databases, one called Progradweb (or part of it as it was a bigger software used in UFSCar) and the other MKCSM is a generic model for a content management system.