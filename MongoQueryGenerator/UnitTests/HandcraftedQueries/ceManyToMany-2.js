db.Person.aggregate([
    {"$lookup": {
        from: 'Owns',
        let: {
            personId: '$_id'
        },
        pipeline: [
            {$match: {
                $expr: {
                    $eq: ['$personId', '$$personId']
                }
            }},
            {$lookup: {
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
                        from: 'Manufacturer',
                        foreignField: '_id',
                        localField: 'manufacturerId',
                        as: 'data_Manufacturer'
                    }},
                    {$unwind: '$data_Manufacturer'},
                    {$addFields: {
                        Car_carId: '$_id',
                        Car_model: '$model',
                        Car_year: '$year',
                        data_ManufacturedBy: [{
                            Manufacturer_manufacturerId: '$data_Manufacturer._id',
                            Manufacturer_name: '$data_Manufacturer.name'
                        }]
                    }},
                    {$project: {
                        _id: false,
                        model: false,
                        year: false,
                        data_Manufacturer: false
                    }}
                ],
                as: 'data_Car'
            }},
            {$unwind: '$data_Car'},
            {$addFields: {
                Owns_ownsId: '$_id'
            }},
            {$replaceRoot: {
                newRoot: {
                    $mergeObjects: ['$data_Car', '$$ROOT']
                }
            }},
            {$project: {
                _id: false,
                personId: false,
                carId: false,
                insuranceId: false,
                data_Car: false,
                manufacturerId: false
            }}
        ],
        as: 'data_Owns'
    }}
]).pretty()