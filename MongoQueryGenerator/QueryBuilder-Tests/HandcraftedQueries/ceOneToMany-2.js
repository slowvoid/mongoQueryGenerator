db.Person.aggregate([
    {"$lookup": {
        from: 'Car',
        let: {
            personId: '$_id'
        },
        pipeline: [
            {$match: {
                $expr: {
                    $eq: ['$driverId', '$$personId']
                }
            }},
            {$lookup: {
                from: 'Repaired',
                let: {
                    carId: '$_id'
                },
                pipeline: [
                    {$match: {
                        $expr: {
                            $eq: ['$carId', '$$carId']
                        }
                    }},
                    {$lookup: {
                        from: 'Garage',
                        foreignField: '_id',
                        localField: 'garageId',
                        as: 'data_Garage'
                    }},
                    {$unwind: '$data_Garage'},
                    {$lookup: {
                        from: 'Supplier',
                        foreignField: '_id',
                        localField: 'supplierId',
                        as: 'data_Supplier'
                    }},
                    {$unwind: '$data_Supplier'},
                    {$addFields: {
                        Repaired_repairedId: '$_id',
                        Repaired_carId: '$carId',
                        Repaired_garageId: '$garageId',
                        Repaired_supplierId: '$supplierId',
                        Repaired_repaired: '$repaired',
                        Garage_garageId: '$data_Garage._id',
                        Garage_name: '$data_Garage.name',
                        Supplier_supplierId: '$data_Supplier._id',
                        Supplier_name: '$data_Supplier.name'
                    }},
                    {$project: {
                        _id: false,
                        carId: false,
                        garageId: false,
                        supplierId: false,
                        repaired: false,
                        data_Garage: false,
                        data_Supplier: false
                    }}
                ],
                as: 'data_Repaired'
            }},
            {$addFields: {
                Car_carId: '$_id',
                Car_model: '$model',
                Car_year: '$year',
                Car_driverId: '$driverId'
            }},
            {$project: {
                _id: false,
                model: false,
                year: false,
                driverId: false
            }}
        ],
        as: 'data_Drives'
    }},
    {"$lookup": {
        from: 'Insurance',
        foreignField: '_id',
        localField: 'insuranceId',
        as: 'data_HasInsuranceJoin'
    }},
    {$unwind: '$data_HasInsuranceJoin'},
    {"$addFields": {
        data_HasInsurance: [{
            Insurance_insuranceId: '$data_HasInsuranceJoin._id',
            Insurance_name: '$data_HasInsuranceJoin.name',
            Insurance_value: '$data_HasInsuranceJoin.value'
        }]
    }},
    {"$project": {
        data_HasInsuranceJoin: false
    }}
]).pretty()