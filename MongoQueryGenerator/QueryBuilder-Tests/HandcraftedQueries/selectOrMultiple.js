db.Person.aggregate([
    {"$match": {
        $expr: {
            $or: [
                { $eq: ['$age', 18] },
                { $eq: ['$age', 21] },
                { $eq: ['$age', 36] }
            ]
        }
    }}
])