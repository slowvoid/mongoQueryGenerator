db.PersonDrives.aggregate([
    {
        "$addFields": {
            "data_Drives.Car_carId": "$drives.carId",
            "data_Drives.Car_carName": "$drives.carName",
            "data_Drives.Car_carYear": "$drives.carYear"
        }
    },
    {
        $project: {
            drives: false
        }
    }
]).pretty()