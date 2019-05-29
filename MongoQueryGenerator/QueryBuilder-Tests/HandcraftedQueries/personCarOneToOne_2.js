db.PersonDrives.aggregate([
    {
        "$addFields": {
            "data_DrivesObj.Car_carId": "$drives.carId",
            "data_DrivesObj.Car_carName": "$drives.carName",
            "data_DrivesObj.Car_carYear": "$drives.carYear",
        }
    },
    {
        $project: {
            drives: false
        }
    },
    {
        "$project": {
            _id: true,
            name: true,
            data_Drives: ['$data_DrivesObj']
        }
    }
]).pretty()