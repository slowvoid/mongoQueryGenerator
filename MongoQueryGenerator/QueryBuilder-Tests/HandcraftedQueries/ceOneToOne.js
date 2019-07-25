﻿db.Person.aggregate([
    {"$lookup": {
        from: 'Car',
        let: {
            carId: '$carId'
        },
        pipeline: [
            {$match: {
                $expr: {
                    $eq: ['$_id', '$$carId']
                }
            }},
            {$lookup: {
                from: 'Repaired',
                let: {
                    carId: '$_id'
                },
                pipeline: [
                    {$match: {
                        $expr: {
                            $eq: ['$carId', '$$carId']
                        }
                    }},
                    {$lookup: {
                        from: 'Garage',
                        foreignField: '_id',
                        localField: 'garageId',
                        as: 'data_Garage'
                    }},
                    {$unwind: '$data_Garage'},
                    {$addFields: {
                        Garage_garageId: '$data_Garage._id',
                        Garage_name: '$data_Garage.name',
                        Repaired_carId: '$carId',
                        Repaired_garageId: '$garageId',
                        Repaired_repaired: '$repaired'
                    }},
                    {$project: {
                        _id: false,
                        data_Garage: false,
                        carId: false,
                        garageId: false,
                        repaired: false
                    }}
                ],
                as: 'data_Repaired'
            }},
            {$addFields: {
                Car_carId: '$_id',
                Car_model: '$model',
                Car_year: '$year'
            }},
            {$project: {
                _id: false,
                model: false,
                year: false
            }}
        ],
        as: 'data_Drives'
    }}
]).pretty()