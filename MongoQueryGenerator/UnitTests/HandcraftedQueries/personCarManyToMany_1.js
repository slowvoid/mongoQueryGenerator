db.Person.aggregate([
    {
        "$lookup": {
            from: 'Insurance',
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
                        foreignField: '_id',
                        localField: 'carId',
                        as: 'data_Car'
                    }
                },
                { $unwind: '$data_Car' },
                {
                    $addFields: {
                        Car_carId: '$data_Car._id',
                        Car_name: '$data_Car.name',
                        Car_year: '$data_Car.year'
                    }
                },
                {
                    $project: {
                        data_Car: false
                    }
                },
                {
                    $addFields: {
                        Insurance_insuranceId: '$_id'
                    }
                },
                {
                    $project: {
                        _id: false,
                        personId: false,
                        carId: false
                    }
                }
            ],
            as: 'data_Insurance'
        }
    }
]).pretty()