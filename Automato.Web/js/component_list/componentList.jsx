var ComponentList = React.createClass({
    getInitialState: function() {
        return {components: []};
    },

    componentWillMount: function() {
        componentStore.getComponents(function(components) {
            this.setState({components: components })
        }.bind(this));
    },

    render: function () {

        var items = this.state.components.map(function(item) {
            return (
                <ComponentListItem component={item} />
            );
        });

        return (
            <div>
                <Link to="addComponent">Add Component</Link>
                <ComponentListSearch />
                <table>
                    <thead>
                    <tr>
                        <th>Name</th>
                        <th>Type</th>
                        <th></th>
                    </tr>
                    </thead>
                    <tbody>
                        {items}
                    </tbody>
                </table>
            </div>
        );
    }
});