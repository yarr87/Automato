var ComponentListSearch = React.createClass({
    getInitialState: function() {
        return {search: ''};
    },

    render: function () {

        return (
            <div>
                <input type="text" ref="search" placeholder="type to search" />
            </div>
        );
    }
});