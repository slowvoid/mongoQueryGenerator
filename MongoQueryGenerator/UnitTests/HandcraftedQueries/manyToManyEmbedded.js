db.Person.aggregate([
    {"$lookup": {
        from: 'Drives',
        let: {
            personId: '$_id'
        },
        pipeline: [
            {$match: {
                $expr: {
                    $eq: ['$personId', '$$personId']
                }
            }},
            {$addFields: {
                Drives_something: '$something',
                Car_carId: '$car.carId',
                Car_model: '$car.model',
                Car_year: '$car.year'
            }},
            {$project: {
                something: false,
                car: false,
                personId: false
            }}
        ],
        as: 'data_Drives'
    }}
]).pretty()