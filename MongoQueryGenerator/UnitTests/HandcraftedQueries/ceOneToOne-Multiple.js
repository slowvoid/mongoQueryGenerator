db.Person.aggregate([
    {
        "$lookup": {
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
                            { $unwind: '$data_Garage' },
                            {
                                $addFields: {
                                    Garage_garageId: '$data_Garage._id',
                                    Garage_name: '$data_Garage.name',
                                    Repaired_repairedId: '$_id',
                                    Repaired_repaired: '$repaired'
                                }
                            },
                            {
                                $lookup: {
                                    from: 'Supplier',
                                    foreignField: '_id',
                                    localField: 'supplierId',
                                    as: 'data_Supplier'
                                }
                            },
                            { $unwind: '$data_Supplier' },
                            {
                                $addFields: {
                                    Supplier_supplierId: '$data_Supplier._id',
                                    Supplier_name: '$data_Supplier.name'
                                }
                            },
                            {
                                $project: {
                                    _id: false,
                                    data_Garage: false,
                                    data_Supplier: false,
                                    carId: false,
                                    garageId: false,
                                    supplierId: false,
                                    repaired: false
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
                        Car_year: '$year'
                    }
                },
                {
                    $project: {
                        _id: false,
                        model: false,
                        year: false
                    }
                }
            ],
            as: 'data_Drives'
        }
    }
]).pretty()