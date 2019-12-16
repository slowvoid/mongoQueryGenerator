db.Person.aggregate([
    {"$match": {
        $expr: {
            $gt: ['$age', 27]
        }
    }}
]).pretty()