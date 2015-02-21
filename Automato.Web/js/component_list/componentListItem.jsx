var ComponentListItem = React.createClass({
    getInitialState: function() {
        return {search: ''};
    },

    render: function () {

        return (
            <tr>
                <td>{this.props.component.name}
                </td>
                <td>{this.props.component.type}</td>
                <td><Link to="editComponent" params={this.props.component}>Edit</Link></td>
            </tr>
        );
    }
});