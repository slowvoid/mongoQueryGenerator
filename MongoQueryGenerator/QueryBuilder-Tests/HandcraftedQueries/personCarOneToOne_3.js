db.PersonDrivesCar.aggregate([
    {
        $project: {
            name: true,
            data_Drives: [
                {
                    Car_carId: '$carId',
                    Car_carName: '$carName',
                    Car_carYear: '$carYear'
                }
            ]
        }
    },
    {
        $project: {
            carId: false,
            carName: false,
            carYear: false
        }
    }
]).pretty()