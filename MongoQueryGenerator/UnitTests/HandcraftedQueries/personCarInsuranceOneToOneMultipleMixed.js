db.Person.aggregate([
    {
        "$addFields": {
            data_Car: {
                Car_name: '$car.name',
                Car_year: '$car.year'
            }
        }
    },
    {
        "$lookup": {
            from: 'Insurance',
            foreignField: '_id',
            localField: 'insuranceId',
            as: 'data_InsuranceJoin'
        }
    },
    { "$unwind": '$data_InsuranceJoin' },
    {
        "$addFields": {
            data_Insurance: {
                Insurance_insuranceId: '$data_InsuranceJoin._id',
                Insurance_name: '$data_InsuranceJoin.name'
            }
        }
    },
    {
        "$addFields": {
            data_HasInsurance: [{
                $mergeObjects: ['$data_Car', '$data_Insurance']
            }]
        }
    },
    {
        "$project": {
            car: false,
            data_Car: false,
            data_Insurance: false,
            data_InsuranceJoin: false,
            insuranceId: false
        }
    }
]).pretty()