﻿db.Person.aggregate([
    {
        "$lookup": {
            from: 'Car',
            foreignField: 'driverId',
            localField: '_id',
            as: 'data_Car'
        }
    },
    {
        "$project": {
            name: true,
            data_Drives: {
                $map: {
                    input: '$data_Car',
                    as: 'car_data',
                    in: {
                        Car_carId: '$$car_data._id',
                        Car_driverId: '$$car_data.driverId',
                        Car_name: '$$car_data.name',
                        Car_year: '$$car_data.year',
                        Drives_drivesFor: '$$car_data.drivesFor'
                    }
                }
            }
        }
    }
]).pretty()