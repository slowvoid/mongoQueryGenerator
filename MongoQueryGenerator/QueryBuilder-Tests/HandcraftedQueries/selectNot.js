db.Person.aggregate([
    {"$match": {
        $expr: {
            $not: {
                $eq: ['$age', 27]
            }
        }
    }}
]).pretty()