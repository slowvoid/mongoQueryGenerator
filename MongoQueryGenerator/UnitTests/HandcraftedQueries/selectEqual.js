db.Person.aggregate([
    {"$match": {
        $expr: {
            $eq: ['$age', 27]
        }
    }}
]).pretty()