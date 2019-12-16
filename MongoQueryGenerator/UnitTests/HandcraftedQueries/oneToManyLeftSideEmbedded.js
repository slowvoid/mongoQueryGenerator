db.Person.aggregate([
    {
        "$lookup": {
            from: 'Insurance',
            foreignField: 'carId',
            localField: 'car.carId',
            as: 'data_HasInsurance'
        }
    },
    {
        "$project": {
            name: true,
            car: true,
            data_HasInsurance: {
                $map: {
                    input: '$data_HasInsurance',
                    as: 'insurance',
                    in: {
                        Insurance_insuranceId: '$$insurance._id',
                        Insurance_name: '$$insurance.name',
                        Insurance_value: '$$insurance.value',
                        Insurance_carId: '$$insurance.carId'
                    }
                }
            }
        }
    }
]).pretty()