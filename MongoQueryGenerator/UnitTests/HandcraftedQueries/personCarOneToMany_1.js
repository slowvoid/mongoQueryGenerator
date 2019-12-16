db.Person.aggregate([
    {
        $lookup: {
            from: 'Car',
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
                    $project: {
                        _id: false,
                        Car_carId: '$_id',
                        Car_name: '$name',
                        Car_year: '$year',
                        Car_personId: '$personId'
                    }
                }
            ],
            as: 'data_Drives'
        }
    }
]).pretty()