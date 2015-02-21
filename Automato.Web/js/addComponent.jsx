var AddComponent = React.createClass({
    mixins: [ReactRouter.State],

    getInitialState: function() {
        return {
            component: {
                name: '',
                type: 'lightSwitch',
                id: 0,
                componentId: ''
            }
        };
    },

    componentWillMount: function() {

        var id = this.getParams().id;

        if (id) {
            componentStore.getComponentById(id, function(component) {
                this.setState({ component: component });
            }.bind(this));
        }
    },

    // This totally sucks, because once you bind a value react forces it to always be that value and ignores input.  The way to update
    // the textbox is to catch the change event and update the underlying data.
    // Tried just setting defaultValue, but that gets set before the data is loaded and then doesn't update again.
    handleNameChange: function(e) {
        this.state.component.name = e.target.value;
        this.setState({component: this.state.component});
    },

    handleIdChange: function(e) {
        this.state.component.componentId = e.target.value;
        this.setState({component: this.state.component});
    },

    handleTypeChange: function(e) {
        this.state.component.type = e.target.value;
        this.setState({component: this.state.component });
    },

    handleClick: function(e) {
        e.preventDefault();

        var component = {
            id: this.state.component.id,
            componentId: this.refs.componentId.getDOMNode().value.trim(),
            name: this.refs.name.getDOMNode().value.trim(),
            type: this.refs.type.getCheckedValue()
        };

        componentStore.addComponent(component);
    },

    render: function () {

        return (
            <div className="row">
            <div className="large-6 columns">
            <form>
                <div className="row">
                    <div className="large-12 columns">
                        <label>Name
                        <input type="text" ref="name" value={this.state.component.name} onChange={this.handleNameChange} required />
                        </label>
                    </div>
                </div>
                <div className="row">
                    <div className="large-12 columns">
                        <label>Component Id
                        <input type="text" ref="componentId"  value={this.state.component.componentId} onChange={this.handleIdChange} required />
                        </label>
                    </div>
                </div>
                <div className="row">
                    <div className="large-12 columns">
                        <label>Type
                        <RadioGroup name="type" ref="type" value="0">
                          <label><input name="lightSwitch" type="radio" value="LightSwitch" checked={this.state.component.type === 'LightSwitch'} onChange={this.handleTypeChange} />Light Switch</label>
                          <label><input name="dimmer" type="radio" value="Dimmer" checked={this.state.component.type === 'Dimmer'} onChange={this.handleTypeChange} />Dimmer</label>
                        </RadioGroup>
                        </label>
                    </div>
                </div>
                <div className="row">
                    <div className="large-12 columns">
                        <button onClick={this.handleClick}>Save</button>
                    </div>
                </div>
            </form>
            </div>
            </div>
        );
    }

});