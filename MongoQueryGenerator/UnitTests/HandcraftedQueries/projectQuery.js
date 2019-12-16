db.Person.aggregate([
    {
        "$lookup": {
            from: 'Owns',
            let: {
                personId: '$_id'
            },
            pipeline: [
                {
                    $match: {
                        $expr: {
                            $eq: ['$personId', '$$personId']
                        }
                    }
                },
                {
                    $lookup: {
                        from: 'Car',
                        let: {
                            carId: '$carId'
                        },
                        pipeline: [
                            {
                                $match: {
                                    $expr: {
                                        $eq: ['$_id', '$$carId']
                                    }
                                }
                            },
                            {
                                $lookup: {
                                    from: 'Manufacturer',
                                    foreignField: '_id',
                                    localField: 'manufacturerId',
                                    as: 'data_Manufacturer'
                                }
                            },
                            { $unwind: '$data_Manufacturer' },
                            {
                                $addFields: {
                                    Car_carId: '$_id',
                                    Car_model: '$model',
                                    Car_year: '$year',
                                    Car_manufacturerId: '$manufacturerId',
                                    data_ManufacturedBy: [{
                                        Manufacturer_manufacturerId: '$data_Manufacturer._id',
                                        Manufacturer_name: '$data_Manufacturer.name'
                                    }]
                                }
                            },
                            {
                                $project: {
                                    _id: false,
                                    model: false,
                                    year: false,
                                    manufacturerId: false,
                                    data_Manufacturer: false
                                }
                            }
                        ],
                        as: 'data_Car'
                    }
                },
                { $unwind: '$data_Car' },
                {
                    $addFields: {
                        Owns_ownsId: '$_id',
                        Owns_personId: '$personId',
                        Owns_carId: '$carId',
                        Owns_insuranceId: '$insuranceId'
                    }
                },
                {
                    $replaceRoot: {
                        newRoot: {
                            $mergeObjects: ['$data_Car', '$$ROOT']
                        }
                    }
                },
                {
                    $project: {
                        _id: false,
                        personId: false,
                        carId: false,
                        insuranceId: false,
                        data_Car: false
                    }
                }
            ],
            as: 'data_Owns'
        }
    },
    {
        "$project": {
            name: true,
            'data_Owns.Car_model': true,
            'data_Owns.Car_year': true,
            'data_Owns.data_ManufacturedBy.Manufacturer_name': true
        }
    }
]).pretty()