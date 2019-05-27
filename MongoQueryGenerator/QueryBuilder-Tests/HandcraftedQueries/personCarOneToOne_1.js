db.Person.aggregate([
    {
        $lookup: {
            from: 'Car',
            foreignField: '_id',
            localField: 'carId',
            as: 'data_Car'
        }
    },
    {
        $project: {
            name: '$name',
            drives_Car: {
                $map: {
                    input: '$data_Car',
                    as: 'car_data',
                    in: {
                        Car_id: '$$car_data._id',
                        Car_name: '$$car_data.name',
                        Car_year: '$$car_data.year'
                    }
                }
            }
        }
    }
]).pretty()