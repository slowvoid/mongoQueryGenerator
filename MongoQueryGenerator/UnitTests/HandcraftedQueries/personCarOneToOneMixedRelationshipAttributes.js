db.Person.aggregate([
    {"$addFields": {
        data_Car: {
            Car_id: '$carId',
            Car_name: '$car.name',
            Car_year: '$car.year'
        }
    }},
    {"$project": {
        carId: false,
        car: false
    }},
    {"$lookup": {
        from: 'Insurance',
        foreignField: '_id',
        localField: 'insuranceId',
        as: 'data_InsuranceJoin'
    }},
    {"$unwind": '$data_InsuranceJoin'},
    {"$addFields": {
        data_Insurance: {
            Insurance_id: '$data_InsuranceJoin._id',
            Insurance_name: '$data_InsuranceJoin.name'
        }
    }},
    {"$project": {
        data_InsuranceJoin: false
    }},
    {"$addFields": {
        data_RelationshipAttributes: {
            HasInsurance_insuranceValue: '$insuranceValue'
        }
    }},
    {"$addFields": {
        data_HasInsurance: [{
            $mergeObjects: ['$data_Insurance', '$data_Car', '$data_RelationshipAttributes']
        }]
    }},
    {"$project": {
        insuranceValue: false,
        data_Insurance: false,
        data_Car: false,
        data_RelationshipAttributes: false
    }}
]).pretty()