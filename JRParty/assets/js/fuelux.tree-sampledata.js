var DataSourceTree = function(options) {
	this._data 	= options.data;
	this._delay = options.delay;
}

DataSourceTree.prototype.data = function(options, callback) {
	var self = this;
	var $data = null;

	if(!("name" in options) && !("type" in options)){
		$data = this._data;//the root tree
		callback({ data: $data });
		return;
	 }
	else if("type" in options && options.type == "folder") {
		if("additionalParameters" in options && "children" in options.additionalParameters)
			$data = options.additionalParameters.children;
		else $data = {}//no data
	}
	
	if($data != null)//this setTimeout is only for mimicking some random delay
		setTimeout(function(){callback({ data: $data });} , parseInt(Math.random() * 500) + 200);

	 
};

var tree_data = {
    'index': { name: '首页', type: 'item' },
	'infor' : {name: '信息登记', type: 'folder'}	,
	'check': { name: '登记审核', type: 'item' },
	'staticts' : {name: '统计', type: 'folder'}	,
	'system' : {name: '运维管理', type: 'folder'}	, 
} 
tree_data['infor']['additionalParameters'] = {
	'children' : {
	    'cars': { name: '填写申请表', type: 'item' },
		'boats' : {name: '修改申请表', type: 'item'}
	}
}
//tree_data['infor']['additionalParameters']['children']['cars']['additionalParameters'] = {
//	'children' : {
//		'classics' : {name: 'Classics', type: 'item'},
//		'convertibles' : {name: 'Convertibles', type: 'item'},
//		'coupes' : {name: 'Coupes', type: 'item'},
//		'hatchbacks' : {name: 'Hatchbacks', type: 'item'},
//		'hybrids' : {name: 'Hybrids', type: 'item'},
//		'suvs' : {name: 'SUVs', type: 'item'},
//		'sedans' : {name: 'Sedans', type: 'item'},
//		'trucks' : {name: 'Trucks', type: 'item'}
//	}
//}

tree_data['check']['additionalParameters'] = {
	'children' : {
		'apartments-rentals' : {name: 'Apartments', type: 'item'},
		'office-space-rentals' : {name: 'Office Space', type: 'item'},
		'vacation-rentals' : {name: 'Vacation Rentals', type: 'item'}
	}
}
tree_data['staticts']['additionalParameters'] = {
	'children' : {
		'apartments' : {name: '年份统计', type: 'item'},
		'villas' : {name: '分数统计', type: 'item'},
		'plots': { name: '区域统计', type: 'item' },
		'depart': { name: '院系专业统计', type: 'item' }
	}
}
tree_data['system']['additionalParameters'] = {
	'children' : {
		'user' : {name: '用户管理', type: 'item'},
		'role' : {name: '角色管理', type: 'item'},
		'linker' : {name: '通讯录管理', type: 'item'},
		'timeSet': { name: '开放时间设置', type: 'item' },
		'note': { name: '登陆日志', type: 'item' }
	}
}

var treeDataSource = new DataSourceTree({data: tree_data});

 