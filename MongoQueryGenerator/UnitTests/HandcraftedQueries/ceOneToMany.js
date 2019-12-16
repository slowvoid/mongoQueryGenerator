db.Person.aggregate([
    {
        "$lookup": {
            from: 'Car',
            let: {
                personId: '$_id'
            },
            pipeline: [
                {
                    $match: {
                        $expr: {
                            $eq: ['$driverId', '$$personId']
                        }
                    }
                },
                {
                    $lookup: {
                        from: 'Repaired',
                        let: {
                            carId: '$_id'
                        },
                        pipeline: [
                            {
                                $match: {
                                    $expr: {
                                        $eq: ['$carId', '$$carId']
                                    }
                                }
                            },
                            {
                                $lookup: {
                                    from: 'Garage',
                                    foreignField: '_id',
                                    localField: 'garageId',
                                    as: 'data_Garage'
                                }
                            },
                            {
                                $unwind: {
                                    path: '$data_Garage',
                                    preserveNullAndEmptyArrays: true
                                }
                            },
                            {
                                $addFields: {
                                    Garage_garageId: '$data_Garage._id',
                                    Garage_name: '$data_Garage.name',
                                    Repaired_repairedId: '$_id',
                                    Repaired_carId: '$carId',
                                    Repaired_garageId: '$garageId',
                                    Repaired_supplierId: '$supplierId',
                                    Repaired_repaired: '$repaired'
                                }
                            },
                            {
                                $project: {
                                    _id: false,
                                    carId: false,
                                    garageId: false,
                                    repaired: false,
                                    supplierId: false,
                                    data_Garage: false
                                }
                            }
                        ],
                        as: 'data_Repaired'
                    }
                },
                {
                    $addFields: {
                        Car_carId: '$_id',
                        Car_model: '$model',
                        Car_year: '$year',
                        Car_driverId: '$driverId'
                    }
                },
                {
                    $project: {
                        _id: false,
                        model: false,
                        year: false,
                        driverId: false
                    }
                }
            ],
            as: 'data_Drives'
        }
    }
]).pretty()