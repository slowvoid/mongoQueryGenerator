﻿db.Person.aggregate([
    {
        "$addFields": {
            data_Car: {
                Car_name: '$car.name',
                Car_year: '$car.year',
                Car_engine: '$carDetails.engine',
                Car_fuel: '$carDetails.fuel'
            }
        }
    },
    {
        "$project": {
            car: false,
            carDetails: false
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
                Insurance_name: '$data_InsuranceJoin.name',
                Insurance_insuranceId: '$data_InsuranceJoin._id'
            }
        }
    },
    {
        "$project": {
            data_InsuranceJoin: false
        }
    },
    {
        "$addFields": {
            data_RelationshipAttributes: {
                HasInsurance_insuranceValue: '$insuranceValue'
            }
        }
    },
    {
        "$addFields": {
            data_HasInsurance: [{
                $mergeObjects: ['$data_Car', '$data_Insurance', '$data_RelationshipAttributes']
            }]
        }
    },
    {
        "$project": {
            data_Car: false,
            data_Insurance: false,
            data_RelationshipAttributes: false,
            insuranceValue: false
        }
    }
]).pretty()