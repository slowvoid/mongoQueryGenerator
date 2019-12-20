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
                                                Repaired_repairedId: '$_id',
                                                Repaired_repaired: '$repaired',
                                                Garage_garageId: '$data_Garage._id',
                                                Garage_name: '$data_Garage.name',
                                                Supplier_supplierId: '$data_Supplier._id',
                                                Supplier_name: '$data_Supplier.name'
                                            }
                                        },
                                        {
                                            $project: {
                                                _id: false,
                                                carId: false,
                                                garageId: false,
                                                supplierId: false,
                                                repaired: false,
                                                data_Garage: false,
                                                data_Supplier: false
                                            }
                                        }
                                    ],
                                    as: 'data_Repaired'
                                }
                            }
                        ],
                        as: 'data_Car'
                    }
                },
                { $unwind: '$data_Car' },
                {
                    $lookup: {
                        from: 'Insurance',
                        foreignField: '_id',
                        localField: 'insuranceId',
                        as: 'data_Insurance'
                    }
                },
                { $unwind: '$data_Insurance' },
                {
                    $addFields: {
                        Owns_ownsId: '$_id',
                        Car_carId: '$data_Car._id',
                        Car_model: '$data_Car.model',
                        Car_year: '$data_Car.year',
                        data_Repaired: '$data_Car.data_Repaired',
                        Insurance_insuranceId: '$data_Insurance._id',
                        Insurance_name: '$data_Insurance.name',
                        Insurance_value: '$data_Insurance.value'
                    }
                },
                {
                    $project: {
                        _id: false,
                        personId: false,
                        carId: false,
                        insuranceId: false,
                        data_Car: false,
                        data_Insurance: false
                    }
                }
            ],
            as: 'data_Owns'
        }
    }
]).pretty()