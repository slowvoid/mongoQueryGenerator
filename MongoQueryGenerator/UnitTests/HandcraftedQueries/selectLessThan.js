db.Person.aggregate([
    {"$match": {
        $expr: {
            $lt: ['$age', 27]
        }
    }}
]).pretty()