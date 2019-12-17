db.Person.aggregate([
    {
        "$project": {
            name: true,
            data_Drives: {
                $map: {
                    input: '$cars_multivalued_',
                    as: 'car_data',
                    in: {
                        Car_name: '$$car_data.name',
                        Car_year: '$$car_data.year',
                        Drives_drivesFor: '$$car_data.drivesFor'
                    }
                }
            }
        }
    }
]).pretty()