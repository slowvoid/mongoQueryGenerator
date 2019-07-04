db.Person.aggregate([
    {"$lookup": {
        from: 'Insurance',
        foreignField: '_id',
        localField: 'car.insuranceId',
        as: 'data_HasInsurance'
    }},
    {"$unwind": '$data_HasInsurance'},
    {"$project": {
        name: true,
        car: true,
        data_HasInsurance: [{
            Insurance_insuranceId: '$data_HasInsurance._id',
            Insurance_name: '$data_HasInsurance.name',
            Insurance_value: '$data_HasInsurance.value'
        }]
    }}
]).pretty()