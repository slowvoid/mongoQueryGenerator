db.PersonDrivesCarMixed.aggregate([
    {
        $project: {
            name: true,
            data_Drives: [
                {
                    Car_carId: '$carId',
                    Car_carName: '$carData.carName',
                    Car_carYear: '$carData.carYear'
                }
            ]
        }
    },
    {
        $project: {
            carId: false,
            carData: false
        }
    }
]).pretty()