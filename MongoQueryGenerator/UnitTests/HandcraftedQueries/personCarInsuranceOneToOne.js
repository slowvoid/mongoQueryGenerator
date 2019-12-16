db.Person.aggregate([
    {
        "$lookup": {
            from: 'Car',
            foreignField: '_id',
            localField: 'carId',
            as: 'data_Car'
        }
    },
    {
        "$project": {
            name: true,
            carId: true,
            insuranceId: true,
            data_MappedCar: {
                $map: {
                    input: '$data_Car',
                    as: 'car_data',
                    in: {
                        Car_carId: '$$car_data._id',
                        Car_name: '$$car_data.name',
                        Car_year: '$$car_data.year'
                    }
                }
            }
        }
    },
    { "$unwind": '$data_MappedCar' },
    {
        "$lookup": {
            from: 'Insurance',
            foreignField: '_id',
            localField: 'insuranceId',
            as: 'data_Insurance'
        }
    },
    {
        "$project": {
            name: true,
            carId: true,
            insuranceId: true,
            data_MappedCar: true,
            data_MappedInsurance: {
                $map: {
                    input: '$data_Insurance',
                    as: 'insurance_data',
                    in: {
                        Insurance_insuranceId: '$$insurance_data._id',
                        Insurance_name: '$$insurance_data.name'
                    }
                }
            }
        }
    },
    { "$unwind": '$data_MappedInsurance' },
    {
        "$addFields": {
            data_HasInsurance: [{
                $mergeObjects: ['$data_MappedCar', '$data_MappedInsurance']
            }]
        }
    },
    {
        "$project": {
            data_MappedCar: false,
            data_MappedInsurance: false
        }
    }
]).pretty()