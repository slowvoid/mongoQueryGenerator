db.Person.aggregate([
    {"$match": {
        $expr: {
            $and: [
                { $eq: ['$age', 27] },
                { $eq: ['$name', 'Summer'] }
            ]
        }
    }}
]).pretty()