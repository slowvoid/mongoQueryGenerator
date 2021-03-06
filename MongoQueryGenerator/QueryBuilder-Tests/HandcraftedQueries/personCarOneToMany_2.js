﻿db.PersonDrivesCars.aggregate([
    {
        "$project": {
            name: true,
            data_Drives: {
                $map: {
                    input: '$cars',
                    as: 'data_car',
                    in: {
                        Car_name: '$$data_car.name',
                        Car_year: '$$data_car.year'
                    }
                }
            }
        }
    }
]).pretty()